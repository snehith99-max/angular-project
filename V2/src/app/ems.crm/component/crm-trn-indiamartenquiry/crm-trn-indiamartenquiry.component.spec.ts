import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CrmTrnIndiamartenquiryComponent } from './crm-trn-indiamartenquiry.component';

describe('CrmTrnIndiamartenquiryComponent', () => {
  let component: CrmTrnIndiamartenquiryComponent;
  let fixture: ComponentFixture<CrmTrnIndiamartenquiryComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [CrmTrnIndiamartenquiryComponent]
    });
    fixture = TestBed.createComponent(CrmTrnIndiamartenquiryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
