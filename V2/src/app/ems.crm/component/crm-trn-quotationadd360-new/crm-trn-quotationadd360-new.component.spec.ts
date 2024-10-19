import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CrmTrnQuotationadd360NewComponent } from './crm-trn-quotationadd360-new.component';

describe('CrmTrnQuotationadd360NewComponent', () => {
  let component: CrmTrnQuotationadd360NewComponent;
  let fixture: ComponentFixture<CrmTrnQuotationadd360NewComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [CrmTrnQuotationadd360NewComponent]
    });
    fixture = TestBed.createComponent(CrmTrnQuotationadd360NewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
