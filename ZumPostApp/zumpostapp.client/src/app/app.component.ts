import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { ApiService } from './api.service';
import { Post } from './post';

interface WeatherForecast {
  date: string;
  temperatureC: number;
  temperatureF: number;
  summary: string;
}


@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent implements OnInit {
  public forecasts: WeatherForecast[] = [];
  public posts: Post[] = [];

  constructor(private http: HttpClient, private apiService: ApiService) {}

  ngOnInit() {
    this.getForecasts();
    this.getPost('tech');
  }

  getForecasts() {
    this.http.get<WeatherForecast[]>('/weatherforecast').subscribe(
      (result) => {
        this.forecasts = result;
      },
      (error) => {
        console.error(error);
      }
    );
  }

  getPost(tag: string): void {
    this.apiService.getPosts(tag).subscribe(posts => {
      console.log('posts...', posts)
      this.posts = posts;
    });

    //this.http.get<Post[]>(`/api/posts/${tag}`).subscribe(
    //  (result) => {
    //    console.log('post result', result)
    //    this.posts = result;
    //  },
    //  (error) => {
    //    console.error(error);
    //  }
    //);
  }

  title = 'zumpostapp.client';
}
