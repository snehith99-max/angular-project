import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ImsTrnStockstatementViewComponent } from './ims-trn-stockstatement-view.component';

describe('ImsTrnStockstatementViewComponent', () => {
  let component: ImsTrnStockstatementViewComponent;
  let fixture: ComponentFixture<ImsTrnStockstatementViewComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ImsTrnStockstatementViewComponent]
    });
    fixture = TestBed.createComponent(ImsTrnStockstatementViewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
