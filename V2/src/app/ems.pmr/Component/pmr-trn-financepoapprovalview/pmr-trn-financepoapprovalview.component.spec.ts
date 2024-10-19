import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PmrTrnFinancepoapprovalviewComponent } from './pmr-trn-financepoapprovalview.component';

describe('PmrTrnFinancepoapprovalviewComponent', () => {
  let component: PmrTrnFinancepoapprovalviewComponent;
  let fixture: ComponentFixture<PmrTrnFinancepoapprovalviewComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [PmrTrnFinancepoapprovalviewComponent]
    });
    fixture = TestBed.createComponent(PmrTrnFinancepoapprovalviewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
