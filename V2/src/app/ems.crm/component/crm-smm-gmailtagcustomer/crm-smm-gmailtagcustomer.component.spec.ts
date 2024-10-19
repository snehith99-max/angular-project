import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CrmSmmGmailtagcustomerComponent } from './crm-smm-gmailtagcustomer.component';

describe('CrmSmmGmailtagcustomerComponent', () => {
  let component: CrmSmmGmailtagcustomerComponent;
  let fixture: ComponentFixture<CrmSmmGmailtagcustomerComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [CrmSmmGmailtagcustomerComponent]
    });
    fixture = TestBed.createComponent(CrmSmmGmailtagcustomerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
