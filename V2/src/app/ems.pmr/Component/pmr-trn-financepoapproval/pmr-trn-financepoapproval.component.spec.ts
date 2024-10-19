import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PmrTrnFinancepoapprovalComponent } from './pmr-trn-financepoapproval.component';

describe('PmrTrnFinancepoapprovalComponent', () => {
  let component: PmrTrnFinancepoapprovalComponent;
  let fixture: ComponentFixture<PmrTrnFinancepoapprovalComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [PmrTrnFinancepoapprovalComponent]
    });
    fixture = TestBed.createComponent(PmrTrnFinancepoapprovalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
