import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SmrMstMaptax2productComponent } from './smr-mst-maptax2product.component';

describe('SmrMstMaptax2productComponent', () => {
  let component: SmrMstMaptax2productComponent;
  let fixture: ComponentFixture<SmrMstMaptax2productComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [SmrMstMaptax2productComponent]
    });
    fixture = TestBed.createComponent(SmrMstMaptax2productComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
