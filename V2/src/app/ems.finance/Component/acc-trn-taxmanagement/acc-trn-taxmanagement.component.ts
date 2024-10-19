import { Component } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { AES } from 'crypto-js';
import { enc } from 'crypto-js';
import { NgxSpinnerService } from 'ngx-spinner';

@Component({
  selector: 'app-acc-trn-taxmanagement',
  templateUrl: './acc-trn-taxmanagement.component.html',
  styleUrls: ['./acc-trn-taxmanagement.component.scss']
})

export class AccTrnTaxmanagementComponent {
  responsedata: any;
  TaxManagementSummary_List: any[] = [];
  parameterValue1: any;
  taxfiling_gid: any;
  showOptionsDivId: any;

  constructor(private formBuilder: FormBuilder, private ToastrService: ToastrService, private router: ActivatedRoute, private route: Router, public service: SocketService, private NgxSpinnerService: NgxSpinnerService) { }

  ngOnInit(): void {
    this.AccTrnTaxManagementsummary();
    document.addEventListener('click', (event: MouseEvent) => {
      if (event.target && !(event.target as HTMLElement).closest('button.btn') && this.showOptionsDivId) {
        this.showOptionsDivId = null;
      }
    });
  }

  AccTrnTaxManagementsummary() {
    this.NgxSpinnerService.show();
    var url = 'TaxManagements/GetTaxManagementSummary'
    this.service.get(url).subscribe((result: any) => {
      $('#TaxManagementSummary_List').DataTable().destroy();
      this.responsedata = result;
      this.TaxManagementSummary_List = result.GetTaxManagementSummary_List;
      this.NgxSpinnerService.hide();
      setTimeout(() => {
        $('#TaxManagementSummary_List').DataTable(
          {
            "pageLength": 50, // Number of rows to display per page
            "lengthMenu": [50, 100, 200, 500], // Dropdown to change page length
          }
        );
      }, 1);
    });
  }

  tax_view(parameter: any) {
    this.parameterValue1 = parameter
    this.taxfiling_gid = this.parameterValue1.fillingref_no;

    const secretKey = 'storyboarderp';
    const taxfiling_gid = AES.encrypt(this.taxfiling_gid, secretKey).toString();
    this.route.navigate(['/finance/AccTrnTaxManagementView', taxfiling_gid]);
  }

  toggleOptions(data: any) {
    if (this.showOptionsDivId === data) {
      this.showOptionsDivId = null;
    } else {
      this.showOptionsDivId = data;
    }
  }
  NavigateToAddTaxFilingPage(){
    this.route.navigate(['/finance/AccTrnTaxFilling']);
        /////For Delete Temp Table Datas///////////////////
        var url = "TaxManagements/TempTableDataDeletions";
        this.service.get(url).subscribe((result: any) => {
        })
        ///////////////////////////////////////////////////
  }
}
