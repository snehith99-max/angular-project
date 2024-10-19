import { Component } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { AES, enc } from 'crypto-js';
import { ActivatedRoute, Router } from '@angular/router';
import { FormBuilder } from '@angular/forms';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { AngularEditorConfig } from '@kolkov/angular-editor';

@Component({
  selector: 'app-otl-trn-materialindent-view',
  templateUrl: './otl-trn-materialindent-view.component.html',
  styleUrls: ['./otl-trn-materialindent-view.component.scss']
})
export class OtlTrnMaterialindentViewComponent {



  config: AngularEditorConfig = {
    editable: false,
    spellcheck: false,
    height: '25rem',
    minHeight: '5rem',
    width: '1000px',
    placeholder: 'Enter text here...',
    translate: 'no',
    defaultParagraphSeparator: 'p',
    defaultFontName: 'Arial',
  };
  materialindentview_list: any[] = [];
  materialindentviewproduct_list: any[] = [];
  customer: any;
  responsedata: any;


  constructor(private formBuilder: FormBuilder, private route: Router, private router: ActivatedRoute, public service: SocketService) { }

  ngOnInit(): void {
    const materialrequisition_gid = this.router.snapshot.paramMap.get('materialrequisition_gid');
    this.customer = materialrequisition_gid;
    const secretKey = 'storyboarderp';
    const deencryptedParam = AES.decrypt(this.customer, secretKey).toString(enc.Utf8);
    console.log(deencryptedParam)
    this.GetMaterialIndentView(deencryptedParam);
    this.GetMaterialIndentViewProduct(deencryptedParam);
  }

  GetMaterialIndentView(materialrequisition_gid: any) {
    var url = 'otltrnMI/GetMaterialIndentView'
    let param = {
      materialrequisition_gid: materialrequisition_gid
    }
    this.service.getparams(url, param).subscribe((result: any) => {
      this.responsedata = result;
      this.materialindentview_list = result.materialindentview_list;
    });
  }
  GetMaterialIndentViewProduct(materialrequisition_gid: any) {
    var url = 'otltrnMI/GetMaterialIndentViewProduct'
    let param = {
      materialrequisition_gid: materialrequisition_gid
    }
    this.service.getparams(url, param).subscribe((result: any) => {
      this.responsedata = result;
      this.materialindentviewproduct_list = result.materialindentviewproduct_list;
    });
  }
                       
  back() {
    this.route.navigate(['/outlet/OtlTrnMaterialindent']);

  }


}







