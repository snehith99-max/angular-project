import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PmrTrnRasieAgreementorderComponent } from './pmr-trn-rasie-agreementorder.component';

describe('PmrTrnRasieAgreementorderComponent', () => {
  let component: PmrTrnRasieAgreementorderComponent;
  let fixture: ComponentFixture<PmrTrnRasieAgreementorderComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [PmrTrnRasieAgreementorderComponent]
    });
    fixture = TestBed.createComponent(PmrTrnRasieAgreementorderComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
