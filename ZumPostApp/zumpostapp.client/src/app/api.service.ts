import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Post } from './post'; 

@Injectable({
  providedIn: 'root'
})
export class ApiService {
  private apiBaseUrl = 'https://localhost:7037/api'; 

  constructor(private http: HttpClient) {
    console.log('http', http.request)
  }

  // function to get all posts filtered by the tag input from the users
  getPosts(tag: string): Observable<Post[]> {
    const posts = this.http.get<Post[]>(`${this.apiBaseUrl}/posts/${tag}`);
    console.log('posts', posts)
    return posts
  }
}
