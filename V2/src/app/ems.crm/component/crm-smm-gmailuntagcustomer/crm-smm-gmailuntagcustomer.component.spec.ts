import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CrmSmmGmailuntagcustomerComponent } from './crm-smm-gmailuntagcustomer.component';

describe('CrmSmmGmailuntagcustomerComponent', () => {
  let component: CrmSmmGmailuntagcustomerComponent;
  let fixture: ComponentFixture<CrmSmmGmailuntagcustomerComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [CrmSmmGmailuntagcustomerComponent]
    });
    fixture = TestBed.createComponent(CrmSmmGmailuntagcustomerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
