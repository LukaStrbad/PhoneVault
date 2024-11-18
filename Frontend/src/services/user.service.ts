import { Injectable } from '@angular/core';
import { environment } from "../environments/environment";
import { HttpClient } from "@angular/common/http";
import { User } from "../model/user";
import { firstValueFrom } from "rxjs";

const url = `${environment.apiUrl}/api/user`;

@Injectable({
  providedIn: 'root'
})
export class UserService {

  constructor(
    private http: HttpClient
  ) { }

  async getAll() {
    const users = await firstValueFrom(this.http.get<User[]>(url))
    users.forEach(user => {
      if (user.createdAt && user.updatedAt) {
        user.createdAt = new Date(user.createdAt);
        user.updatedAt = new Date(user.updatedAt);
      }
    });
    return users;
  }

  async get(id: number) {
    const user = await firstValueFrom(this.http.get<User>(`${url}/${id}`));
    if (user.createdAt && user.updatedAt) {
      user.createdAt = new Date(user.createdAt);
      user.updatedAt = new Date(user.updatedAt);
    }
    return user;
  }

  async update(userId: string, isAdmin: boolean) {
    return await firstValueFrom(this.http.post<User>(`${url}/${userId}`, isAdmin));
  }
}
