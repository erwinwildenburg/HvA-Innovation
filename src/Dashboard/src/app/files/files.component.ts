import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute, Params } from '@angular/router';
import { StoredFileInfo } from '../../models/storedfileinfo';
import { FilesService } from './files.service';
import { Observable } from 'rxjs/Observable';

@Component({
  selector: 'app-files',
  templateUrl: './files.component.html',
  styleUrls: ['./files.component.css']
})
export class FilesComponent implements OnInit {

  public files: StoredFileInfo[];
  private selectedId: string;

  constructor(
    private router: Router,
    private route: ActivatedRoute,
    private filesService: FilesService,
  ) { }

  ngOnInit() {
    this.filesService.getFiles(null)
      .subscribe((files: StoredFileInfo[]) => this.files = files);
  }

  openFile(file) {
    this.router.navigate(['edit/' + file.key]);
  }
}
