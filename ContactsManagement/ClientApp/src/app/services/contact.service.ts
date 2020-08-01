import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Contact } from '../models/contact.model';

@Injectable()
export class ContactService {
  apiUrl: string;

  constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: any) {
    this.apiUrl = baseUrl + 'api';
  }

  getAllContacts() {
    return this.http.get<Contact[]>(this.apiUrl + '/contacts');
  }

  getContactById(contactId: number) {
    return this.http.get<Contact>(this.apiUrl + '/contacts/' + contactId);
  }

  addContact(contact: Contact) {
    return this.http.post<Contact>(this.apiUrl + '/contacts', contact);
  }

  updateContact(contact: Contact) {
    return this.http.put<Contact>(this.apiUrl + '/contacts', contact);
  }

  deleteContact(contactId: number) {
    return this.http.delete<any>(this.apiUrl + '/contacts/' + contactId);
  }
}
