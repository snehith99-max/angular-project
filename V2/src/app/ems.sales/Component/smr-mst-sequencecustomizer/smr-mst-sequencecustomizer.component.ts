import { Component, OnInit, OnDestroy, ChangeDetectorRef } from '@angular/core';
import { FormBuilder, FormGroup, Validators, FormControl } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { AES } from 'crypto-js';
import { DomSanitizer, SafeUrl } from '@angular/platform-browser';
import { Subscription, Observable } from 'rxjs';
import { ActivatedRoute, Router } from '@angular/router';
import { SocketService } from '../../../ems.utilities/services/socket.service';
import { environment } from 'src/environments/environment';
import { ExcelService } from 'src/app/Service/excel.service';
import { NgxSpinnerService } from 'ngx-spinner';

@Component({
  selector: 'app-smr-mst-sequencecustomizer',
  templateUrl: './smr-mst-sequencecustomizer.component.html',
  styleUrls: ['./smr-mst-sequencecustomizer.component.scss']
})
export class SmrMstSequencecustomizerComponent {

  
  response_data: any;
  sequenceCodeList: any[] = [];

  constructor(private fb: FormBuilder, private sanitizer: DomSanitizer, private excelService: ExcelService, public NgxSpinnerService: NgxSpinnerService, private route: ActivatedRoute, private router: Router, private service: SocketService, private ToastrService: ToastrService) {
   
  }
  ngOnInit(): void {
    this.GetSequenceCodeCustomizer();
  }
  GetSequenceCodeCustomizer() {
    var api = 'SmrMstSequenceCodeCustomizer/GetSequenceCodeCustomizer';
    this.NgxSpinnerService.show()
    this.service.get(api).subscribe((result: any) => {
      $('#sequenceCodeList').DataTable().destroy();
      this.response_data = result;
      this.sequenceCodeList = this.response_data.SequenceCodeSummary;
     
      this.NgxSpinnerService.hide()
      setTimeout(() => {
        $('#sequenceCodeList').DataTable();

      }, 1);
    });

  }
  SequenceEdit(sequencecodecustomizer_gid: any){
debugger
    const secretKey = 'storyboarderp';
    const param = (sequencecodecustomizer_gid);
    const encryptedParam = AES.encrypt(param, secretKey).toString();
    this.router.navigate(['/smr/SmrMstSequencecodeedit', encryptedParam])
  }
}
