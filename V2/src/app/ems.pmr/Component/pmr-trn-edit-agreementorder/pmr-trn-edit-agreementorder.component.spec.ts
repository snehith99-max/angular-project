import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PmrTrnEditAgreementorderComponent } from './pmr-trn-edit-agreementorder.component';

describe('PmrTrnEditAgreementorderComponent', () => {
  let component: PmrTrnEditAgreementorderComponent;
  let fixture: ComponentFixture<PmrTrnEditAgreementorderComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [PmrTrnEditAgreementorderComponent]
    });
    fixture = TestBed.createComponent(PmrTrnEditAgreementorderComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
