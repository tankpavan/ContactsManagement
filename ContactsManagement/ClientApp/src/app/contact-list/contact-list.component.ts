import { Component, OnInit, TemplateRef } from '@angular/core';
import { Contact } from '../models/contact.model';
import { ContactService } from '../services/contact.service';
import { BsModalService, BsModalRef } from 'ngx-bootstrap/modal';
import { Ng4LoadingSpinnerService } from 'ng4-loading-spinner';
import { AddContactComponent } from '../add-contact/add-contact.component';
import { EditContactComponent } from '../edit-contact/edit-contact.component';

@Component({
  selector: 'app-contact-list',
  templateUrl: './contact-list.component.html'
})
export class ContactListComponent implements OnInit {
  bsModalRef: BsModalRef;
  bsConfirmModalRef: BsModalRef;
  contacts: Contact[];
  selectedContactId: number;
  alerts: any = [];
  columns = [
    { header: "First Name", field: "FirstName" },
    { header: "Last Name", field: "LastName" },
    { header: "Email", field: "Email" },
    { header: "Phone Number", field: "PhoneNumber" }
  ];

  constructor(private contactService: ContactService, private modalService: BsModalService, private spinner: Ng4LoadingSpinnerService) {

  }

  ngOnInit() {
    this.getAllContacts();
  }

  getAllContacts() {
    this.spinner.show();
    this.contactService.getAllContacts().subscribe(result => {
      this.contacts = result;
      this.spinner.hide();
    },
      error => {
        this.alerts.push({ type: 'danger', message: 'Something went wrong. Please try again later.', dismissible: 'true', timeout: '5000' });
        this.spinner.hide();
      }
    );
  }

  openAddContactDialog() {
    const initialState = {
      contacts: this.contacts,
      alerts: this.alerts
    };
    this.bsModalRef = this.modalService.show(AddContactComponent, { initialState: initialState, keyboard: false, ignoreBackdropClick: true });
    let subscriber = this.modalService.onHide.subscribe(r => {
      subscriber.unsubscribe();
      this.contacts = this.bsModalRef.content.contacts;
      this.alerts = this.bsModalRef.content.alerts;
    });
  }

  openEditContactDialog(contactId: number) {
    const initialState = {
      contactId: contactId,
      contacts: this.contacts,
      alerts: this.alerts
    };
    this.bsModalRef = this.modalService.show(EditContactComponent, { initialState: initialState, keyboard: false, ignoreBackdropClick: true });
    let subscriber = this.modalService.onHide.subscribe(r => {
      subscriber.unsubscribe();
      this.contacts = this.bsModalRef.content.contacts;
      this.alerts = this.bsModalRef.content.alerts;
    });
  }

  openDeleteConfirmationDialog(deleteConfirmationDialog: TemplateRef<any>, contactId: number) {
    this.bsConfirmModalRef = this.modalService.show(deleteConfirmationDialog, { class: 'modal-sm', ignoreBackdropClick: true, keyboard: false });
    this.selectedContactId = contactId;
  }

  confirmDelete() {
    this.spinner.show();
    this.contactService.deleteContact(this.selectedContactId).subscribe(result => {
      this.alerts.push({ type: 'success', message: result.Message, dismissible: 'true', timeout: '5000' });
      this.refreshContactList();
    },
      error => {
        this.alerts.push({ type: 'danger', message: 'Something went wrong. Please try again later.', dismissible: 'true', timeout: '5000' });
        this.bsConfirmModalRef.hide();
        this.spinner.hide();
      }
    );
  }

  cancelDelete() {
    this.selectedContactId = 0;
    this.bsConfirmModalRef.hide();
  }

  refreshContactList() {
    const index = this.contacts.findIndex(item => item.ContactId === this.selectedContactId);
    if (index > -1)
      this.contacts.splice(index, 1);

    this.bsConfirmModalRef.hide();
    this.spinner.hide();
  }
}

