import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { query } from '@angular/core/src/render3/instructions';
import { StoredFile, StoredFileInfo } from '../../models/storedfileinfo';
import { EditService } from './edit.service';
import { Observable } from 'rxjs/Observable';

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

  constructor(private route: ActivatedRoute, private editService: EditService) {}

  ngOnInit() {
    this.pathFile = this.route.params.subscribe(params => this.getFile(params));
  }

  getFile(params) {
    this.editService.getFile(params).subscribe((file: StoredFile[]) => {
      Object.assign(this.file, this.file, file);
      this.getContent(params);
    });
  }

  getContent(params) {
    this.editService.getContent(params).subscribe((content: string) => this.file.content = content);
  }

  storeFile(file) {
    this.editService.storeFile(file).subscribe((res: object) => console.log(res));
  }

}
