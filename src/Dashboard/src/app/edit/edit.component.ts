import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-edit',
  templateUrl: './edit.component.html',
  styleUrls: ['./edit.component.css']
})
export class EditComponent implements OnInit {

  constructor() {}

  file = {
    key: 'Key 1',
    title: 'Title 1',
    info: {
      create: new Date().toString(),
      edit: new Date().toString(),
      lang: 'dutch'
    },
    content: 'Hello there, content here'
  };


  ngOnInit() {}

}
