import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { AngularEditorConfig } from '@kolkov/angular-editor';
import { NgxSpinnerService } from 'ngx-spinner';


@Component({
  selector: 'app-sys-mst-template-add',
  templateUrl: './sys-mst-template-add.component.html',
  styleUrls: ['./sys-mst-template-add.component.scss']
})

export class SysMstTemplateAddComponent {
  templateform: any;
  responsedata: any;
  templatetype_list: any;
  mdltemplatetype_gid: any;
  config: AngularEditorConfig = {
    editable: true,
    spellcheck: true,
    height: '12rem',
    minHeight: '0rem',
    //width: '1080px',
    placeholder: 'Enter text here...',
    translate: 'no',
    defaultParagraphSeparator: 'p',
    defaultFontName: 'Arial',
  };

  ngOnInit(): void {

    var api = 'SysMstTemplate/GetTemplateType';
    this.service.get(api).subscribe((result: any) => {
      this.responsedata = result;
      this.templatetype_list = this.responsedata.GetTemplateTypedropdown;
    });
  }

  constructor(public service: SocketService, private route: Router, public NgxSpinnerService: NgxSpinnerService, private ToastrService: ToastrService) {

    this.templateform = new FormGroup({
      template_name: new FormControl('', [Validators.required, Validators.pattern(/^\S.*$/)]),
      templatetype_gid: new FormControl('', [Validators.required]),
      template_content: new FormControl('', [Validators.required, 
        Validators.pattern(/^\S.*$/)
      ]),
    })
  }

  get template_name() {
    return this.templateform.get('template_name');
  }

  get templatetypeControl() {
    return this.templateform.get('templatetype_gid');
  }

  get template_content() {
    return this.templateform.get('template_content');
  }

  templatesubmit() {
    var api = 'SysMstTemplate/PostTemplate';
    this.NgxSpinnerService.show();

    this.service.post(api, this.templateform.value).subscribe((result: any) => {
      //this.responsedata = result;
      if (result.status == true) {
        this.ToastrService.success(result.message)
        this.NgxSpinnerService.hide();
        this.route.navigate(['/system/SysMstTemplate']);

      }
      else {
        this.ToastrService.warning(result.message);
        this.NgxSpinnerService.hide();

        this.templateform.reset();
      }
    });
   
  }

  back() {
    this.route.navigate(['/system/SysMstTemplate']);
  }
}
