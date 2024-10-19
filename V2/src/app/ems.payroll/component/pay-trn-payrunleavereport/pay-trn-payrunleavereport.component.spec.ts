import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PayTrnPayrunleavereportComponent } from './pay-trn-payrunleavereport.component';

describe('PayTrnPayrunleavereportComponent', () => {
  let component: PayTrnPayrunleavereportComponent;
  let fixture: ComponentFixture<PayTrnPayrunleavereportComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [PayTrnPayrunleavereportComponent]
    });
    fixture = TestBed.createComponent(PayTrnPayrunleavereportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
