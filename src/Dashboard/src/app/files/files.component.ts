import { Component, OnInit } from '@angular/core';
import { RouterModule, Router } from '@angular/router';

@Component({
  selector: 'app-files',
  templateUrl: './files.component.html',
  styleUrls: ['./files.component.css']
})
export class FilesComponent implements OnInit {

  constructor(private router: Router) { }

  files = [
    {
      key: 'Key 1',
      title: 'Title 1',
      info: {
        create: new Date().toString(),
        edit: new Date().toString(),
        lang: 'dutch'
      }
    },
    {
      key: 'Key 2',
      title: 'Title 2',
      info: {
        create: new Date().toString(),
        edit: new Date().toString(),
        lang: 'dutch'
      }
    },
    {
      key: 'Key 3',
      title: 'Title 3',
      info: {
        create: new Date().toString(),
        edit: new Date().toString(),
        lang: 'dutch'
      }
    },
    {
      key: 'Key 4',
      title: 'Title 4',
      info: {
        create: new Date().toString(),
        edit: new Date().toString(),
        lang: 'dutch'
      }
    },
    {
      key: 'Key 5',
      title: 'Title 5',
      info: {
        create: new Date().toString(),
        edit: new Date().toString(),
        lang: 'dutch'
      }
    },
    {
      key: 'Key 6',
      title: 'Title 6',
      info: {
        create: new Date().toString(),
        edit: new Date().toString(),
        lang: 'dutch'
      }
    },
  ];

  ngOnInit() {
  }

  openFile(file) {
    this.router.navigate(['edit/' + file.key]);
  }
}
