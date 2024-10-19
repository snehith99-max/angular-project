import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CrmSmmSmslogComponent } from './crm-smm-smslog.component';

describe('CrmSmmSmslogComponent', () => {
  let component: CrmSmmSmslogComponent;
  let fixture: ComponentFixture<CrmSmmSmslogComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [CrmSmmSmslogComponent]
    });
    fixture = TestBed.createComponent(CrmSmmSmslogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
