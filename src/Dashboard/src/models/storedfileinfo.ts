export class StoredFile {
  public key: string;
  public title: string;
  public info: StoredFileInfo;
}

export class StoredFileInfo {
  public lang: string;
  public create: string;
  public edit: string;
}
