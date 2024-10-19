import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { Options } from 'flatpickr/dist/types/options';
import flatpickr from 'flatpickr';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';
import { DatePipe } from '@angular/common';
import { SelectionModel } from '@angular/cdk/collections';

export class INPUTTAX {
  InputTaxSummary_List: string[] = [];
}

export class OUTPUTTAX {
  OutputTaxSummary_List: string[] = [];
}

export class CREDITNOTETAX {
  CreditNoteTaxSummary_List: string[] = [];
}

export class DEBITNOTETAX {
  DebitNoteTaxSummary_List: string[] = [];
}

@Component({
  selector: 'app-acc-trn-taxfilling',
  templateUrl: './acc-trn-taxfilling.component.html',
  styleUrls: ['./acc-trn-taxfilling.component.scss']
})

export class AccTrnTaxfillingComponent {
  pick: Array<any> = [];
  CurObj: INPUTTAX = new INPUTTAX();
  selection = new SelectionModel<INPUTTAX>(true, []);
  CurObj1: OUTPUTTAX = new OUTPUTTAX();
  selection1 = new SelectionModel<OUTPUTTAX>(true, []);
  CurObj2: CREDITNOTETAX = new CREDITNOTETAX();
  selection2 = new SelectionModel<CREDITNOTETAX>(true, []);
  CurObj3: DEBITNOTETAX = new DEBITNOTETAX();
  selection3 = new SelectionModel<DEBITNOTETAX>(true, []);
  reactiveform: FormGroup | any;
  reactiveform1: FormGroup | any;
  response_data: any;
  from_date: any;
  to_date: any;
  search_id: any;

  InputTaxSummary_List: any[] = [];
  OutputTaxSummary_List: any[] = [];
  CreditNoteTaxSummary_List: any[] = [];
  DebitNoteTaxSummary_List: any[] = [];
  responsedata: any;
  CompileInputTax_List: any;
  CompileOutputTax_List: any;
  CompileCreditTax_List: any;
  CompileDebitTax_List: any;
  TaxCalculation_List: any;

  constructor(public service: SocketService, private router: ActivatedRoute, private route: Router,
    private FormBuilder: FormBuilder, private NgxSpinnerService: NgxSpinnerService,
    private ToastrService: ToastrService, private datePipe: DatePipe,) {
  }

  ngOnInit(): void {
    const options: Options = {
      dateFormat: 'd-m-Y',
    };
    flatpickr('.date-picker', options);

    this.reactiveform = new FormGroup({
      from_date: new FormControl(''),
      to_date: new FormControl(''),
    });

    this.reactiveform1 = new FormGroup({
      output_tax: new FormControl(''),
      credit_tax: new FormControl(''),
      totaloutputcredit_tax: new FormControl(''),
      debit_tax: new FormControl(''),
      input_tax: new FormControl(''),
      totalinputdebit_tax: new FormControl(''),
      tax_payable: new FormControl(''),
      interestcharge_payable: new FormControl(''),
      penality_payable: new FormControl(''),
      totaltax: new FormControl(''),
    });

    this.AccTrnCompileInputTax();
    this.AccTrnCompileOutputTax();
    this.AccTrnCompileCreditTax();
    this.AccTrnCompileDebitTax();
  }

  compile() {
    this.AccTrnCompileInputTax();
    this.AccTrnCompileOutputTax();
    this.AccTrnCompileCreditTax();
    this.AccTrnCompileDebitTax();
    this.AccTrnCompileTaxCalculation();
  }

  AccTrnCompileTaxCalculation() {
    this.NgxSpinnerService.show();
    var url = 'TaxManagements/GetTaxCalculation'
    this.service.get(url).subscribe((result: any) => {
      $('#TaxCalculation_List').DataTable().destroy();
      this.responsedata = result;
      this.TaxCalculation_List = result.GetTaxCalculation_List;
      this.NgxSpinnerService.hide();
    });
  }

  AccTrnCompileInputTax() {
    this.NgxSpinnerService.show();
    var url = 'TaxManagements/GetCompileInputTax'
    this.service.get(url).subscribe((result: any) => {
      $('#CompileInputTax_List').DataTable().destroy();
      this.responsedata = result;
      this.CompileInputTax_List = result.GetCompileInputTax_List;
      this.NgxSpinnerService.hide();
    });
  }

  AccTrnCompileOutputTax() {
    this.NgxSpinnerService.show();
    var url = 'TaxManagements/GetCompileOutputTax'
    this.service.get(url).subscribe((result: any) => {
      $('#CompileOutputTax_List').DataTable().destroy();
      this.responsedata = result;
      this.CompileOutputTax_List = result.GetCompileOutputTax_List;
      this.NgxSpinnerService.hide();
    });
  }

  AccTrnCompileCreditTax() {
    this.NgxSpinnerService.show();
    var url = 'TaxManagements/GetCompileCreditTax'
    this.service.get(url).subscribe((result: any) => {
      $('#CompileCreditTax_List').DataTable().destroy();
      this.responsedata = result;
      this.CompileCreditTax_List = result.GetCompileCreditTax_List;
      this.NgxSpinnerService.hide();
    });
  }

  AccTrnCompileDebitTax() {
    this.NgxSpinnerService.show();
    var url = 'TaxManagements/GetCompileDebitTax'
    this.service.get(url).subscribe((result: any) => {
      $('#CompileDebitTax_List').DataTable().destroy();
      this.responsedata = result;
      this.CompileDebitTax_List = result.GetCompileDebitTax_List;
      this.NgxSpinnerService.hide();
    });
  }

  isAllSelected() {
    const numSelected = this.selection.selected.length;
    const numRows = this.InputTaxSummary_List.length;
    return numSelected === numRows;
  }

  masterToggle() {
    this.isAllSelected() ?
      this.selection.clear() :
      this.InputTaxSummary_List.forEach((row: INPUTTAX) => this.selection.select(row));
    this.calculateSelectedTaxAmount();
  }

  isAllSelected1() {
    const numSelected = this.selection1.selected.length;
    const numRows = this.OutputTaxSummary_List.length;
    return numSelected === numRows;
  }

  masterToggle1() {
    this.isAllSelected1() ?
      this.selection1.clear() :
      this.OutputTaxSummary_List.forEach((row: OUTPUTTAX) => this.selection1.select(row));
    this.calculateSelectedTaxAmount1();
  }

  isAllSelected2() {
    const numSelected = this.selection2.selected.length;
    const numRows = this.CreditNoteTaxSummary_List.length;
    return numSelected === numRows;
  }

  masterToggle2() {
    this.isAllSelected2() ?
      this.selection2.clear() :
      this.CreditNoteTaxSummary_List.forEach((row: CREDITNOTETAX) => this.selection2.select(row));
    this.calculateSelectedTaxAmount2();
  }

  isAllSelected3() {
    const numSelected = this.selection3.selected.length;
    const numRows = this.DebitNoteTaxSummary_List.length;
    return numSelected === numRows;
  }

  masterToggle3() {
    this.isAllSelected3() ?
      this.selection3.clear() :
      this.DebitNoteTaxSummary_List.forEach((row: DEBITNOTETAX) => this.selection3.select(row));
    this.calculateSelectedTaxAmount3();
  }

  calculateSelectedTaxAmount(): string {
    const totalAmount1 = this.selection.selected.reduce((total: number, data: any) => {
      const inputTotalTax = parseFloat(data.Taxable_Amount.replace(/,/g, ''));
      if (!isNaN(inputTotalTax)) {
        return total + inputTotalTax;
      }
      return total;
    }, 0);

    const formatter = new Intl.NumberFormat('en-US', {
      minimumFractionDigits: 2,
      maximumFractionDigits: 2
    });
    return formatter.format(totalAmount1);
  }

  calculateSelectedTaxAmount1(): string {
    const totalAmount2 = this.selection1.selected.reduce((total: number, data: any) => {
      const outputtotaltax = parseFloat(data.Taxable_Amount.replace(/,/g, ''));
      if (!isNaN(outputtotaltax)) {
        return total + outputtotaltax;
      }
      return total;
    }, 0);

    const formatter = new Intl.NumberFormat('en-US', {
      minimumFractionDigits: 2,
      maximumFractionDigits: 2
    });

    const totalAmountFormatted = formatter.format(totalAmount2);
    return totalAmountFormatted;
  }

  calculateSelectedTaxAmount2(): string {
    const totalAmount3 = this.selection2.selected.reduce((total: number, data: any) => {
      const creditnotetotaltax = parseFloat(data.Taxable_Amount.replace(/,/g, ''));
      if (!isNaN(creditnotetotaltax)) {
        return total + creditnotetotaltax;
      }
      return total;
    }, 0);

    const formatter = new Intl.NumberFormat('en-US', {
      minimumFractionDigits: 2,
      maximumFractionDigits: 2
    });

    const totalAmountFormatted = formatter.format(totalAmount3);
    return totalAmountFormatted;
  }

  calculateSelectedTaxAmount3(): string {
    const totalAmount4 = this.selection3.selected.reduce((total: number, data: any) => {
      const debitnotetotaltax = parseFloat(data.Taxable_Amount.replace(/,/g, ''));
      if (!isNaN(debitnotetotaltax)) {
        return total + debitnotetotaltax;
      }
      return total;
    }, 0);

    const formatter = new Intl.NumberFormat('en-US', {
      minimumFractionDigits: 2,
      maximumFractionDigits: 2
    });

    const totalAmountFormatted = formatter.format(totalAmount4);
    return totalAmountFormatted;
  }

  inputtax_submit() {
    this.pick = this.selection.selected
    let list = this.pick
    this.CurObj.InputTaxSummary_List = list
    console.log(this.CurObj)
    if (this.CurObj.InputTaxSummary_List.length != 0) {

      this.NgxSpinnerService.show();
      var url1 = 'TaxManagements/InputTaxSubmit'
      this.service.post(url1, this.CurObj).pipe().subscribe((result: any) => {
        if (result.status == false) {
          window.scrollTo({
            top: 0,
          });
          this.NgxSpinnerService.hide();
          this.ToastrService.warning(result.message)
        }
        else {
          window.scrollTo({
            top: 0,
          });
          this.reactiveform.value.from_date = null!;
          this.reactiveform.value.to_date = null!;
          window.location.reload();
          this.NgxSpinnerService.hide();
          this.selection.clear();
          this.ToastrService.success(result.message)
        }
      });
    }
    else {
      this.ToastrService.warning("Kindly Select Atleast One Record! ")
    }
  }

  outputtax_submit() {
    this.pick = this.selection1.selected
    let list = this.pick
    this.CurObj1.OutputTaxSummary_List = list
    console.log(this.CurObj1)
    if (this.CurObj1.OutputTaxSummary_List.length != 0) {
      this.NgxSpinnerService.show();
      var url1 = 'TaxManagements/OutputTaxSubmit'
      this.service.post(url1, this.CurObj1).pipe().subscribe((result: any) => {
        if (result.status == false) {
          window.scrollTo({
            top: 0,
          });
          this.NgxSpinnerService.hide();
          this.ToastrService.warning(result.message)
        }
        else {
          window.scrollTo({
            top: 0,
          });
          this.reactiveform.value.from_date = null!;
          this.reactiveform.value.to_date = null!;
          window.location.reload();
          this.NgxSpinnerService.hide();
          this.selection1.clear();
          this.ToastrService.success(result.message)
        }
      });
    }
    else {
      this.ToastrService.warning("Kindly Select Atleast One Record! ")
    }
  }

  creditnotetax_submit() {
    this.pick = this.selection2.selected
    let list = this.pick
    this.CurObj2.CreditNoteTaxSummary_List = list
    console.log(this.CurObj2)
    if (this.CurObj2.CreditNoteTaxSummary_List.length != 0) {

      this.NgxSpinnerService.show();
      var url1 = 'TaxManagements/CreditNoteTaxSubmit'
      this.service.post(url1, this.CurObj2).pipe().subscribe((result: any) => {
        if (result.status == false) {
          window.scrollTo({
            top: 0,
          });
          this.NgxSpinnerService.hide();
          this.ToastrService.warning(result.message)
        }
        else {
          window.scrollTo({
            top: 0,
          });
          this.reactiveform.value.from_date = null!;
          this.reactiveform.value.to_date = null!;
          window.location.reload();

          this.NgxSpinnerService.hide();
          this.selection2.clear();
          this.ToastrService.success(result.message)
        }
      });
    }
    else {
      this.ToastrService.warning("Kindly Select Atleast One Record! ")
    }
  }

  debitnotetax_submit() {
    this.pick = this.selection3.selected
    let list = this.pick
    this.CurObj3.DebitNoteTaxSummary_List = list
    console.log(this.CurObj3)
    if (this.CurObj3.DebitNoteTaxSummary_List.length != 0) {

      this.NgxSpinnerService.show();
      var url1 = 'TaxManagements/DebitNoteTaxSubmit'
      this.service.post(url1, this.CurObj3).pipe().subscribe((result: any) => {
        if (result.status == false) {
          window.scrollTo({
            top: 0,
          });
          this.NgxSpinnerService.hide();
          this.ToastrService.warning(result.message)
        }
        else {
          window.scrollTo({
            top: 0,
          });
          this.reactiveform.value.from_date = null!;
          this.reactiveform.value.to_date = null!;
          window.location.reload();

          this.NgxSpinnerService.hide();
          this.selection2.clear();
          this.ToastrService.success(result.message)
        }
      });
    }
    else {
      this.ToastrService.warning("Kindly Select Atleast One Record! ")
    }
  }

  Overall_submit() {
    var params = {
      CompileInputTax_List: this.CompileInputTax_List,
      CompileOutputTax_List: this.CompileOutputTax_List,
      CompileCreditTax_List: this.CompileCreditTax_List,
      TaxCalculation_List: this.TaxCalculation_List,
      interestcharge_payable: this.reactiveform1.value.interestcharge_payable,
      penality_payable: this.reactiveform1.value.penality_payable
    }

    var url = 'TaxManagements/PostTaxOverallSubmit'
    this.NgxSpinnerService.show();
    this.service.postparams(url, params).subscribe((result: any) => {
      this.NgxSpinnerService.hide();
      if (result.status == false) {
        window.scrollTo({
          top: 0,
        });
        this.ToastrService.warning(result.message)
      }
      else {
        this.ToastrService.success(result.message)
        this.route.navigate(['/finance/AccTrnTaxManagement']);
      }
    });
  }

  onSearchClick(param: any) {
    this.search_id = param;
    if (this.search_id == '1') {
      if ((this.reactiveform.value.from_date == "" && this.reactiveform.value.to_date == "") ||
        (this.reactiveform.value.from_date != "" && this.reactiveform.value.to_date == "") ||
        (this.reactiveform.value.from_date == "" && this.reactiveform.value.to_date != "")) {
        this.ToastrService.warning('Kindly Fill All Mandatory Fields !! ')
      }
      else {
        this.reactiveform.value;
        this.from_date = this.reactiveform.value.from_date;
        this.to_date = this.reactiveform.value.to_date;
        this.NgxSpinnerService.show();
        var url = 'AccTrnBankbooksummary/GetInputTaxSummary';
        let params = {
          from_date: this.from_date,
          to_date: this.to_date
        }
        this.service.getparams(url, params).subscribe((result: any) => {
          $('#InputTaxSummary_List').DataTable().destroy();
          this.response_data = result;
          this.InputTaxSummary_List = this.response_data.GetInputTaxSummaryList;
          this.NgxSpinnerService.hide();
        });
      }
    }
    else if (this.search_id == '2') {
      if ((this.reactiveform.value.from_date == "" && this.reactiveform.value.to_date == "") ||
        (this.reactiveform.value.from_date != "" && this.reactiveform.value.to_date == "") ||
        (this.reactiveform.value.from_date == "" && this.reactiveform.value.to_date != "")) {
        this.ToastrService.warning('Kindly Fill All Mandatory Fields !! ')
      }
      else {
        this.reactiveform.value;
        this.from_date = this.reactiveform.value.from_date;
        this.to_date = this.reactiveform.value.to_date;
        this.NgxSpinnerService.show();
        var url = 'AccTrnBankbooksummary/GetOutputTaxSummary';
        let params = {
          from_date: this.from_date,
          to_date: this.to_date
        }
        this.service.getparams(url, params).subscribe((result: any) => {
          $('#OutputTaxSummary_List').DataTable().destroy();
          this.response_data = result;
          this.OutputTaxSummary_List = this.response_data.GetOutputTaxSummaryList;
          this.NgxSpinnerService.hide();
        });
      }
    }
    else if (this.search_id == '3') {
      if ((this.reactiveform.value.from_date == "" && this.reactiveform.value.to_date == "") ||
        (this.reactiveform.value.from_date != "" && this.reactiveform.value.to_date == "") ||
        (this.reactiveform.value.from_date == "" && this.reactiveform.value.to_date != "")) {
        this.ToastrService.warning('Kindly Fill All Mandatory Fields !! ')
      }
      else {
        this.reactiveform.value;
        this.from_date = this.reactiveform.value.from_date;
        this.to_date = this.reactiveform.value.to_date;
        this.NgxSpinnerService.show();
        var url = 'AccTrnBankbooksummary/GetCreditNoteTaxSummary';
        let params = {
          from_date: this.from_date,
          to_date: this.to_date
        }
        this.service.getparams(url, params).subscribe((result: any) => {
          $('#CreditNoteTaxSummary_List').DataTable().destroy();
          this.response_data = result;
          this.CreditNoteTaxSummary_List = this.response_data.GetCreditNoteTaxSummaryList;
          this.NgxSpinnerService.hide();
        });
      }
    }
    else if (this.search_id == '4') {
      if ((this.reactiveform.value.from_date == "" && this.reactiveform.value.to_date == "") ||
        (this.reactiveform.value.from_date != "" && this.reactiveform.value.to_date == "") ||
        (this.reactiveform.value.from_date == "" && this.reactiveform.value.to_date != "")) {
        this.ToastrService.warning('Kindly Fill All Mandatory Fields !! ')
      }
      else {
        this.reactiveform.value;
        this.from_date = this.reactiveform.value.from_date;
        this.to_date = this.reactiveform.value.to_date;
        this.NgxSpinnerService.show();
        var url = 'AccTrnBankbooksummary/GetDebitNoteTaxSummary';
        let params = {
          from_date: this.from_date,
          to_date: this.to_date
        }
        this.service.getparams(url, params).subscribe((result: any) => {
          $('#DebitNoteTaxSummary_List').DataTable().destroy();
          this.response_data = result;
          this.DebitNoteTaxSummary_List = this.response_data.GetDebitNoteTaxSummaryList;
          this.NgxSpinnerService.hide();
        });
      }
    }
    else {
      this.reactiveform.value.from_date = null!;
      this.reactiveform.value.to_date = null!;
      window.location.reload();
    }
  }

  onrefreshclick() {
    this.reactiveform.value.from_date = null!;
    this.reactiveform.value.to_date = null!;
    window.location.reload();
  }

  back() {
    this.route.navigate(['/finance/AccTrnTaxManagement']);
  }
}
