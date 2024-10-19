import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ImsTrnStocktransferBranchwiseComponent } from './ims-trn-stocktransfer-branchwise.component';

describe('ImsTrnStocktransferBranchwiseComponent', () => {
  let component: ImsTrnStocktransferBranchwiseComponent;
  let fixture: ComponentFixture<ImsTrnStocktransferBranchwiseComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ImsTrnStocktransferBranchwiseComponent]
    });
    fixture = TestBed.createComponent(ImsTrnStocktransferBranchwiseComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
