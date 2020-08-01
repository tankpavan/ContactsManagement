import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, FormControl, Validator, Validators } from '@angular/forms';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { Ng4LoadingSpinnerService } from 'ng4-loading-spinner';
import { Contact } from '../models/contact.model';
import { ContactService } from '../services/contact.service';

@Component({
  selector: 'app-add-contact',
  templateUrl: './add-contact.component.html'
})
export class AddContactComponent implements OnInit {
  alerts: any = [];
  contact: Contact = new Contact();
  contacts: Contact[];
  contactForm: FormGroup;
  namePattern: RegExp = /^[a-zA-Z ]+$/;
  numberPattern: RegExp = /^[0-9]{10}$/;

  submitted: boolean = false;

  constructor(private contactService: ContactService, private formBuilder: FormBuilder, private bsModalRef: BsModalRef, private spinner: Ng4LoadingSpinnerService) {
    this.contactForm = this.formBuilder.group({
      'FirstName': new FormControl('', Validators.compose([Validators.required, Validators.pattern(this.namePattern)])),
      'LastName': new FormControl('', Validators.compose([Validators.required, Validators.pattern(this.namePattern)])),
      'Email': new FormControl('', Validators.compose([Validators.required, Validators.email])),
      'PhoneNumber': new FormControl('', Validators.compose([Validators.required, Validators.pattern(this.numberPattern)])),
      'Status': new FormControl('')
    });
  }

  ngOnInit() { }

  onSubmit(value: any) {
    if (this.contactForm.valid) {
      this.spinner.show();
      value.Status = "Active";
      this.contactService.addContact(value).subscribe(result => {
        this.contact = result;
        this.alerts.push({ type: 'success', message: 'Contact added successfully.', dismissible: 'true', timeout: '5000' });
        this.refreshContactList();
      },
        error => {
          this.alerts.push({ type: 'danger', message: 'Something went wrong. Please try again later.', dismissible: 'true', timeout: '5000' });
          this.refreshContactList();
        }
      );
    }
  }

  onCancel() {
    this.bsModalRef.hide();
    this.contactForm.reset();
  }

  refreshContactList() {
    if (this.contact && this.contact.ContactId > 0)
      this.contacts.push(this.contact);
    this.bsModalRef.hide();
    this.spinner.hide();
  }

  validateEmail() {
    if (this.contactForm.controls['Email'].value) {
      if (this.contacts.some(c => c.Email.toLowerCase() == this.contactForm.controls['Email'].value)) {
        this.contactForm.controls['Email'].setErrors({ 'AlreadyExist': 'AlreadyExist' });
        this.contactForm.controls['Email'].markAsDirty();
      }
      else {
        this.contactForm.controls['Email'].setErrors(null);
        this.contactForm.controls['Email'].markAsPristine();
      }
    }
  }

  validatePhoneNumber() {
    if (this.contactForm.controls['PhoneNumber'].value) {
      if (this.contacts.some(c => c.PhoneNumber == this.contactForm.controls['PhoneNumber'].value)) {
        this.contactForm.controls['PhoneNumber'].setErrors({ 'AlreadyExist': 'AlreadyExist' });
        this.contactForm.controls['PhoneNumber'].markAsDirty();
      }
      else {
        this.contactForm.controls['PhoneNumber'].setErrors(null);
        this.contactForm.controls['PhoneNumber'].markAsPristine();
      }
    }
  }
}

