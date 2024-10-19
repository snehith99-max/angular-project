import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ImsTrnStockstatementComponent } from './ims-trn-stockstatement.component';

describe('ImsTrnStockstatementComponent', () => {
  let component: ImsTrnStockstatementComponent;
  let fixture: ComponentFixture<ImsTrnStockstatementComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ImsTrnStockstatementComponent]
    });
    fixture = TestBed.createComponent(ImsTrnStockstatementComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
