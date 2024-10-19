import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ImsTrnSalesreturnAddselectComponent } from './ims-trn-salesreturn-addselect.component';

describe('ImsTrnSalesreturnAddselectComponent', () => {
  let component: ImsTrnSalesreturnAddselectComponent;
  let fixture: ComponentFixture<ImsTrnSalesreturnAddselectComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ImsTrnSalesreturnAddselectComponent]
    });
    fixture = TestBed.createComponent(ImsTrnSalesreturnAddselectComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
