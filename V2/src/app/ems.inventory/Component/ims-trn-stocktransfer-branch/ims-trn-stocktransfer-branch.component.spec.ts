import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ImsTrnStocktransferBranchComponent } from './ims-trn-stocktransfer-branch.component';

describe('ImsTrnStocktransferBranchComponent', () => {
  let component: ImsTrnStocktransferBranchComponent;
  let fixture: ComponentFixture<ImsTrnStocktransferBranchComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ImsTrnStocktransferBranchComponent]
    });
    fixture = TestBed.createComponent(ImsTrnStocktransferBranchComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
