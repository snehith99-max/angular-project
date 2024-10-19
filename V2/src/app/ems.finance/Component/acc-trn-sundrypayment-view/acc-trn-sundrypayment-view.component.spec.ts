import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AccTrnSundrypaymentViewComponent } from './acc-trn-sundrypayment-view.component';

describe('AccTrnSundrypaymentViewComponent', () => {
  let component: AccTrnSundrypaymentViewComponent;
  let fixture: ComponentFixture<AccTrnSundrypaymentViewComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [AccTrnSundrypaymentViewComponent]
    });
    fixture = TestBed.createComponent(AccTrnSundrypaymentViewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
