import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { query } from '@angular/core/src/render3/instructions';

@Component({
  selector: 'app-edit',
  templateUrl: './edit.component.html',
  styleUrls: ['./edit.component.css']
})
export class EditComponent implements OnInit {
  pathFile: any;
  file = {
    key: null,
    title: null,
    info: {
      create: null,
      edit: null,
      lang: null,
    },
    content: null,
  };

  constructor(private route: ActivatedRoute, private http: HttpClient) {}

  ngOnInit() {
    this.pathFile = this.route.params.subscribe(params => this.getFile(params));
  }

  getFile(params) {
    this.http.post('http://127.0.0.1:8081/getFile', {
      key: params.key,
      title: params.title
    })
    .subscribe(
      res => {
        if (res && (res as any).Item) {
          Object.assign(this.file, this.file, (res as any).Item);
          this.getContent(params);
        }
      },
      err => {
        console.log('Error occured');
      }
    );
  }

  getContent(params) {
    this.http.post('http://127.0.0.1:8081/getFileContent', {
      key: params.key
    })
    .subscribe(
      res => {
        if (res) {
          console.log(res);
          this.file.content = res;
        }
      },
      err => {
        console.log('Error occured');
      }
    );
  }

  storeFile(file) {
    this.http.post('http://127.0.0.1:8081/storeFile', file)
    .subscribe(
      res => {
        console.log(res);
      },
      err => {
        console.log('Error occured');
      }
    );
  }

}
