import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SmrTrnQuotationaddNewComponent } from './smr-trn-quotationadd-new.component';

describe('SmrTrnQuotationaddNewComponent', () => {
  let component: SmrTrnQuotationaddNewComponent;
  let fixture: ComponentFixture<SmrTrnQuotationaddNewComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [SmrTrnQuotationaddNewComponent]
    });
    fixture = TestBed.createComponent(SmrTrnQuotationaddNewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
