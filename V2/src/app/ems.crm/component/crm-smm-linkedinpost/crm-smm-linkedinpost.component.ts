import { Component, OnInit, ElementRef, ViewChild } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { AES, enc } from 'crypto-js';
import { DatePipe } from '@angular/common';
import { NgxSpinnerService } from 'ngx-spinner';
import { saveAs } from 'file-saver';
import { Location } from '@angular/common';
import { TabsModule } from 'ngx-bootstrap/tabs';
import { TabsetComponent } from 'ngx-bootstrap/tabs';

@Component({
  selector: 'app-crm-smm-linkedinpost',
  templateUrl: './crm-smm-linkedinpost.component.html',
  styleUrls: ['./crm-smm-linkedinpost.component.scss']
})
export class CrmSmmLinkedinpostComponent {
  account_id:any;
  responsedata: any;
  postsummarylistview: any;
  id:any;
  lscompany_name: any;
  lstotal_post: any;

  constructor(private fb: FormBuilder, private route: Router, public service: SocketService, private router: ActivatedRoute, private ToastrService: ToastrService, private datePipe: DatePipe, private NgxSpinnerService: NgxSpinnerService, private location: Location) {

  }
  ngOnInit(): void {
    const account_id = this.router.snapshot.paramMap.get('account_id');
    this.account_id = account_id;
    const secretKey = 'storyboarderp';
    const deencryptedParam = AES.decrypt(this.account_id, secretKey).toString(enc.Utf8);
    this.account_id = deencryptedParam
    this.Postsummarydetails(deencryptedParam);
  }
  postviewrefresh(account_id: any) {
    // Show spinner
    this.NgxSpinnerService.show();

    var url1 = 'Linkedin/GetLinkedinpost'
    let param = {
      account_id: account_id
    };
    this.service.getparams(url1, param).subscribe((result: any) => {
      this.responsedata = result;
      setTimeout(() => {
        this.NgxSpinnerService.hide();
        window.location.reload();
      }, 5000);
    });
  }
  Postsummarydetails(deencryptedParam: any) {
    this.NgxSpinnerService.show();
    var url = 'Linkedin/GetLinkedinpostsummary'
    let params = {
      account_id: deencryptedParam
    }
    this.service.getparams(url, params).subscribe((result: any) => {
      this.NgxSpinnerService.hide();
      this.responsedata = result;
      this.postsummarylistview = this.responsedata.postsummarylistview;
      this.lscompany_name = this.responsedata.lscompany_name;
      this.lstotal_post = this.responsedata.lstotal_post;

      setTimeout(() => {
        $('#postsummarylistview').DataTable();
      }, 10000);
    });
  }
  downloadImage(imagedownload_url: string, media_type: string) {
    if (imagedownload_url != null && imagedownload_url != "") {
      if (media_type === 'Image' || media_type === 'Multi Images') {
        saveAs(imagedownload_url, this.id + '.png');
      }
      else if (media_type === 'Video') {
        saveAs(imagedownload_url, this.id + '.mp4');
      }
      else {
        saveAs(imagedownload_url, this.id + '.png');
      }
    }
    else {
      window.scrollTo({
        top: 0, // Code is used for scroll top after event done
      });
      this.ToastrService.warning('No Image Found')

    }

  }
}
