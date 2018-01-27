import { Component } from '@angular/core';

@Component({
  templateUrl: './home.component.html'
})
export class HomeComponent {
  file = {
    name: '<FILENAME>',
    // tslint:disable-next-line:max-line-length
    content: 'Lorem ipsum dolor sit amet, consectetur adipiscing elit. Vestibulum convallis est nec molestie dignissim. Cras dui leo, aliquet vel eros et, faucibus lacinia justo. Duis eu nisl et tellus facilisis consequat. Nullam euismod, tellus aliquam faucibus ultrices, diam lectus pellentesque nunc, ut vulputate nibh ex in leo. Aenean fermentum, nibh a mollis maximus, elit nisi viverra libero, nec placerat nisi nisl id massa. Lorem ipsum dolor sit amet, consectetur adipiscing elit. Integer quis risus lacus. Duis molestie enim vitae vehicula varius. Ut ut ex nisl. Praesent blandit luctus sem, sed porttitor tellus hendrerit vitae. Vestibulum ac dictum dui. Ut accumsan lorem nec ante mattis ultricies. Praesent fermentum convallis nunc, vitae sagittis tellus ornare tincidunt.',
    version: 1.0,
    language: 'english'
  };

  storeFile = function (object) {
    object.version += 1;
    console.log(object);
  };

  translateFile = function (object, lang) {
    if (object.language === lang) {
      return;
    }
  };

  // const translate = require('google-translate-api');
  // const self = this;
  // translate(object.content, {
  //   from: object.language,
  //   to: lang
  // }).then(res => {
  //   this.file.content = res.from.language.iso;
  // }).catch(err => {
  //   console.error(err);
  // });
}
