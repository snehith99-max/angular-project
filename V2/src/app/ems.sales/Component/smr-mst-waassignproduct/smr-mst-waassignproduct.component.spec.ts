import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SmrMstWaassignproductComponent } from './smr-mst-waassignproduct.component';

describe('SmrMstWaassignproductComponent', () => {
  let component: SmrMstWaassignproductComponent;
  let fixture: ComponentFixture<SmrMstWaassignproductComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [SmrMstWaassignproductComponent]
    });
    fixture = TestBed.createComponent(SmrMstWaassignproductComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
