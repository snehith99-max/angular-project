import { Component } from '@angular/core';
import { FormBuilder, FormGroup,} from '@angular/forms';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { ActivatedRoute, Router } from '@angular/router';
import { NgxSpinnerService } from 'ngx-spinner';
import { AES } from 'crypto-js';
@Component({
  selector: 'app-ims-trn-delivery-acknowledgement',
  templateUrl: './ims-trn-delivery-acknowledgement.component.html',
  styleUrls: ['./ims-trn-delivery-acknowledgement.component.scss']
})
export class ImsTrnDeliveryAcknowledgementComponent  {
  responsedata: any;
  deliverysummary_list: any[] = [];
  getData: any;
  constructor(private formBuilder: FormBuilder,public NgxSpinnerService: NgxSpinnerService,public service :SocketService,private route:Router,private ToastrService: ToastrService) {
  }

  ngOnInit(): void {
    this.GetImsTrnDeliveryAcknowledgementSummary();
}
// // //// Summary Grid//////
GetImsTrnDeliveryAcknowledgementSummary() {
  debugger
  var url = 'ImsTrnDeliveryAcknowledgement/GetImsTrnDeliveryAcknowledgementSummary'
  this.NgxSpinnerService.show()
   this.service.get(url).subscribe((result: any) => {
     this.responsedata = result;
     this.deliverysummary_list = result.deliverysummary_list;

     
     setTimeout(() => {
       $('#deliverysummary_list').DataTable();
             }, 1);
             this.NgxSpinnerService.hide()


  })
 
 
}
splitIntoLines(text: string, lineLength: number): string[] {
  const lines = [];
  for (let i = 0; i < text.length; i += lineLength) {
    lines.push(text.substr(i, lineLength));
  }
  return lines;
}
onview(params:any){
  const secretKey = 'storyboarderp';
  const param = (params);
  const encryptedParam = AES.encrypt(param,secretKey).toString();
  this.route.navigate(['/ims/ImsTrnDeliveryview',encryptedParam]);
}
}
