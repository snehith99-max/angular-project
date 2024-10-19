import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AES } from 'crypto-js';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';

@Component({
  selector: 'app-pbl-trn-raisedebitnote-select',
  templateUrl: './pbl-trn-raisedebitnote-select.component.html',
})
export class PblTrnRaisedebitnoteSelectComponent {


  Getraisedebitnote_list: any[]=[];

  constructor(private serivce : SocketService,
    private route : ActivatedRoute,
    private router : Router
  ){}
  
  ngOnInit() : void {
    var summaryapi = 'PblDebitNote/GetRaiseDebitNote';
    this.serivce.get(summaryapi).subscribe((result : any)=> {
      this.Getraisedebitnote_list = result.Getraisedebitnote_list;
      setTimeout(()=>{
        $('#Getraisedebitnote_list').DataTable()        
      }, 1);
    });
  }
  raisedebitnote(invoice_gid: any){
    debugger
    const key = 'storyboard';
    const param = invoice_gid;
    const invoicegid = AES.encrypt(param, key).toString();
    this.router.navigate(['/payable/PblTrnRaiseDebitNoteSelect', invoicegid])
  }
}
