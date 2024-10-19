import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PmrTrnCreatecontractComponent } from './pmr-trn-createcontract.component';

describe('PmrTrnCreatecontractComponent', () => {
  let component: PmrTrnCreatecontractComponent;
  let fixture: ComponentFixture<PmrTrnCreatecontractComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [PmrTrnCreatecontractComponent]
    });
    fixture = TestBed.createComponent(PmrTrnCreatecontractComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
