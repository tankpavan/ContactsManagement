import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, FormControl, Validator, Validators } from '@angular/forms';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { Ng4LoadingSpinnerService } from 'ng4-loading-spinner';
import { Contact } from '../models/contact.model';
import { ContactService } from '../services/contact.service';

@Component({
  selector: 'app-edit-contact',
  templateUrl: './edit-contact.component.html'
})
export class EditContactComponent implements OnInit {
  alerts: any = [];
  contactId: number;
  contact: Contact = new Contact();
  contacts: Contact[];
  contactForm: FormGroup;
  namePattern: RegExp = /^[a-zA-Z ]+$/;
  numberPattern: RegExp = /^[0-9]{10}$/;

  submitted: boolean = false;

  constructor(private contactService: ContactService, private formBuilder: FormBuilder, private bsModalRef: BsModalRef, private spinner: Ng4LoadingSpinnerService) {
    this.contactForm = this.formBuilder.group({
      'ContactId': new FormControl(''),
      'FirstName': new FormControl('', Validators.compose([Validators.required, Validators.pattern(this.namePattern)])),
      'LastName': new FormControl('', Validators.compose([Validators.required, Validators.pattern(this.namePattern)])),
      'Email': new FormControl('', Validators.compose([Validators.required, Validators.email])),
      'PhoneNumber': new FormControl('', Validators.compose([Validators.required, Validators.pattern(this.numberPattern)])),
      'Status': new FormControl('')
    });
  }

  ngOnInit() {
    this.spinner.show();
    this.contactService.getContactById(this.contactId).subscribe(result => {
      this.contact = result;
      this.contactForm.controls['ContactId'].setValue(this.contact.ContactId);
      this.contactForm.controls['FirstName'].setValue(this.contact.FirstName);
      this.contactForm.controls['LastName'].setValue(this.contact.LastName);
      this.contactForm.controls['Email'].setValue(this.contact.Email);
      this.contactForm.controls['PhoneNumber'].setValue(this.contact.PhoneNumber);
      this.contactForm.controls['Status'].setValue(this.contact.Status);
      this.spinner.hide();
    },
      error => {
        this.alerts.push({ type: 'danger', message: 'Something went wrong. Please try again later.', dismissible: 'true', timeout: '5000' });
        this.bsModalRef.hide();
        this.spinner.hide();
      }
    );
  }

  onSubmit(value: any) {
    if (this.contactForm.valid) {
      this.spinner.show();
      this.contactService.updateContact(value).subscribe(result => {
        this.contact = result;
        this.alerts.push({ type: 'success', message: 'Contact updated successfully.', dismissible: 'true', timeout: '5000' });
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
  }

  refreshContactList() {
    const index = this.contacts.findIndex(item => item.ContactId === this.contact.ContactId);
    if (index > -1)
      this.contacts[index] = this.contact;

    this.bsModalRef.hide();
    this.spinner.hide();
  }

  validateEmail() {
    if (this.contactForm.controls['Email'].value) {
      if (this.contacts.some(c => c.Email.toLowerCase() == this.contactForm.controls['Email'].value && c.ContactId != this.contact.ContactId)) {
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
    console.log(1);
    if (this.contactForm.controls['PhoneNumber'].value) {
      if (this.contacts.some(c => c.PhoneNumber == this.contactForm.controls['PhoneNumber'].value && c.ContactId != this.contact.ContactId)) {
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

