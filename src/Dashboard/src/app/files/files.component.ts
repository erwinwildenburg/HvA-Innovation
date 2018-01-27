import { Component, OnInit } from '@angular/core';
import { RouterModule, Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-files',
  templateUrl: './files.component.html',
  styleUrls: ['./files.component.css']
})
export class FilesComponent implements OnInit {

  constructor(private router: Router, private http: HttpClient) {}

  newFile = '';
  files = [];

  ngOnInit() {
    this.getAllFiles();
  }

  getAllFiles() {
    this.http.post('http://127.0.0.1:8081/getAllFiles', {})
      .subscribe(
        res => {
          if (res && (res as any).Items) {
            this.files = (res as any).Items;
          }
          console.log(res);
        },
        err => {
          console.log('Error occured');
        }
      );

    this.newFile = '';
  }

  createFile() {
    this.http.post('http://127.0.0.1:8081/createFile', {
        key: new Date().getTime().toString(),
        title: this.newFile
      })
      .subscribe(
        res => {
          if (res) {
            this.getAllFiles();
          }
        },
        err => {
          console.log('Error occured');
        }
      );

    this.newFile = '';
  }

  deleteFile(file) {
    this.http.post('http://127.0.0.1:8081/deleteFile', {
      key: file.key,
      title: file.title
    })
      .subscribe(
        res => {
          if (res) {
            this.getAllFiles();
          }
          console.log(res);
        },
        err => {
          console.log('Error occured');
        }
      );
  }

  openFile(file) {
    this.router.navigate(['edit/', file.key, file.title]);
  }

}
