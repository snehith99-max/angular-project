import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CrmSmmSmscontactlogComponent } from './crm-smm-smscontactlog.component';

describe('CrmSmmSmscontactlogComponent', () => {
  let component: CrmSmmSmscontactlogComponent;
  let fixture: ComponentFixture<CrmSmmSmscontactlogComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [CrmSmmSmscontactlogComponent]
    });
    fixture = TestBed.createComponent(CrmSmmSmscontactlogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
