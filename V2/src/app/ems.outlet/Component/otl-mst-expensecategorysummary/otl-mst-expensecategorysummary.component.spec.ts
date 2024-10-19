import { ComponentFixture, TestBed } from '@angular/core/testing';

import { OtlMstExpensecategorysummaryComponent } from './otl-mst-expensecategorysummary.component';

describe('OtlMstExpensecategorysummaryComponent', () => {
  let component: OtlMstExpensecategorysummaryComponent;
  let fixture: ComponentFixture<OtlMstExpensecategorysummaryComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [OtlMstExpensecategorysummaryComponent]
    });
    fixture = TestBed.createComponent(OtlMstExpensecategorysummaryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
