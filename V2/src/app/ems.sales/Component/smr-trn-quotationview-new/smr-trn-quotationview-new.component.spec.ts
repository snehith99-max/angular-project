import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SmrTrnQuotationviewNewComponent } from './smr-trn-quotationview-new.component';

describe('SmrTrnQuotationviewNewComponent', () => {
  let component: SmrTrnQuotationviewNewComponent;
  let fixture: ComponentFixture<SmrTrnQuotationviewNewComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [SmrTrnQuotationviewNewComponent]
    });
    fixture = TestBed.createComponent(SmrTrnQuotationviewNewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
