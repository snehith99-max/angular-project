import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SmrTrnQuotationaddNew360Component } from './smr-trn-quotationadd-new360.component';

describe('SmrTrnQuotationaddNew360Component', () => {
  let component: SmrTrnQuotationaddNew360Component;
  let fixture: ComponentFixture<SmrTrnQuotationaddNew360Component>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [SmrTrnQuotationaddNew360Component]
    });
    fixture = TestBed.createComponent(SmrTrnQuotationaddNew360Component);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
