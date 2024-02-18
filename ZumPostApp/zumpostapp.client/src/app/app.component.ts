import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { ApiService } from './api.service';
import { Post } from './post';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrl: './app.component.css',
})

export class AppComponent implements OnInit {
  public posts: Post[] = [];

  selectedTag: string = 'Tech';
  tagOptions: string[] = ['Tech', 'Design', 'Culture', 'Science'];

  selectedSortBy: string = 'Id';
  sortByOptions: string[] = ['Id', 'Reads', 'Likes', 'Popularity'];
  
  selectedDirection: string = 'Asc';
  directionOptions: string[] = ['Asc', 'Desc'];

  constructor(private http: HttpClient, private apiService: ApiService) {}

  ngOnInit() {
    this.getPost('tech');
  }

  handleFormData():void{
    console.log('selectedSortBy', this.selectedSortBy)

    // get the values of the form and call the getPost to filter the results
    const tag:string = this.selectedTag.toLowerCase()
    const sortBy:string = this.selectedSortBy.toLowerCase()
    const direction: string = this.selectedDirection.toLowerCase()

    // call getPost function
    this.getPost(tag, sortBy, direction)
  }

  getPost(tag: string, sortBy?: string, direction?: string): void {
    this.apiService.getPosts(tag, sortBy, direction).subscribe(
      posts => {
        this.posts = posts;
      console.log('posts...', this.posts)

      }, error => {
        console.error(error);
      }

    );

  }

  title = 'zumpostapp.client';
}
