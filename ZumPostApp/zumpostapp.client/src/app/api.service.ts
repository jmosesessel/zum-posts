import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Post } from './post'; 

@Injectable({
  providedIn: 'root'
})
export class ApiService {
  private apiUrl = 'https://localhost:7037/api/Posts'; // Adjust the API URL accordingly

  constructor(private http: HttpClient) {
    console.log('http', http.request)
  }

  getPosts(tag: string): Observable<Post[]> {
    const posts = this.http.get<Post[]>(`${this.apiUrl}/${tag}`);
    console.log('posts', posts)
    return posts
  }
}
