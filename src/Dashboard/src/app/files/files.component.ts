import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute, Params } from '@angular/router';
import { StoredFile, StoredFileInfo } from '../../models/storedfileinfo';
import { FilesService } from './files.service';
import { Observable } from 'rxjs/Observable';

@Component({
  selector: 'app-files',
  templateUrl: './files.component.html',
  styleUrls: ['./files.component.css']
})
export class FilesComponent implements OnInit {

  constructor(private router: Router, private filesService: FilesService) {}

  newFile = '';
  files = [];

  ngOnInit() {
    this.getAllFiles();
  }

  getAllFiles() {
    this.filesService.getAllFiles().subscribe((files: StoredFile[]) => this.files = files);
  }

  createFile() {
    this.filesService.createFile({title: this.newFile}).subscribe((files: StoredFile[]) => this.getAllFiles());
    this.newFile = '';
  }

  deleteFile(file) {
    this.filesService.deleteFile(file).subscribe((files: StoredFile[]) => this.getAllFiles());
  }

  openFile(file) {
    this.router.navigate(['edit/', file.key, file.title]);
  }

}
