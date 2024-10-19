import { ComponentFixture, TestBed } from '@angular/core/testing';

import { OtlMstUsereaddComponent } from './otl-mst-usereadd.component';

describe('OtlMstUsereaddComponent', () => {
  let component: OtlMstUsereaddComponent;
  let fixture: ComponentFixture<OtlMstUsereaddComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [OtlMstUsereaddComponent]
    });
    fixture = TestBed.createComponent(OtlMstUsereaddComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
