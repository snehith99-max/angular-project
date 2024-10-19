import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PmrTrnRaiseEnquiryaddNewComponent } from './pmr-trn-raise-enquiryadd-new.component';

describe('PmrTrnRaiseEnquiryaddNewComponent', () => {
  let component: PmrTrnRaiseEnquiryaddNewComponent;
  let fixture: ComponentFixture<PmrTrnRaiseEnquiryaddNewComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [PmrTrnRaiseEnquiryaddNewComponent]
    });
    fixture = TestBed.createComponent(PmrTrnRaiseEnquiryaddNewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
