import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PmrMstMaptax2productComponent } from './pmr-mst-maptax2product.component';

describe('SmrMstMaptax2productComponent', () => {
  let component: PmrMstMaptax2productComponent;
  let fixture: ComponentFixture<PmrMstMaptax2productComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [PmrMstMaptax2productComponent]
    });
    fixture = TestBed.createComponent(PmrMstMaptax2productComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
