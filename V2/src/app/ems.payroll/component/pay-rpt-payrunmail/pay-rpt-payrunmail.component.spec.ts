import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PayRptPayrunmailComponent } from './pay-rpt-payrunmail.component';

describe('PayRptPayrunmailComponent', () => {
  let component: PayRptPayrunmailComponent;
  let fixture: ComponentFixture<PayRptPayrunmailComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [PayRptPayrunmailComponent]
    });
    fixture = TestBed.createComponent(PayRptPayrunmailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
