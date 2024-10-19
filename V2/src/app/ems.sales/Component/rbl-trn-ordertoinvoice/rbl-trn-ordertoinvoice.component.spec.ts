import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RblTrnOrdertoinvoiceComponent } from './rbl-trn-ordertoinvoice.component';

describe('RblTrnOrdertoinvoiceComponent', () => {
  let component: RblTrnOrdertoinvoiceComponent;
  let fixture: ComponentFixture<RblTrnOrdertoinvoiceComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [RblTrnOrdertoinvoiceComponent]
    });
    fixture = TestBed.createComponent(RblTrnOrdertoinvoiceComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
